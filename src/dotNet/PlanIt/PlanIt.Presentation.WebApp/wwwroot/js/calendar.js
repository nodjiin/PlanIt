// UI elements
let modal;
let userListItems;
let newUsernameInput;
let addButton;
let saveButton;
let monthp;
let prevMonthArr;
let nextMonthArr;

// view data
let currentUser;
let dateElements;
let statusCache = new Map();
let fdMonth;

// server side data
let months;
let minDate;
let maxDate;
let planId;

// single date status
const busy = 0;
const available = 1;
const disabled = 2;
const outOfRange = 3;

// apiUrls
let userApiUrl;

class DateElement {
    #status = busy;
    #element;
    constructor(de) {
        this.#element = de;
        this.#element.addEventListener("click", (w) => this.#handleClick(w));
    }
    getStatus() {
        return this.#status;
    }
    updateStatus(s) {
        switch (s) {
            case busy:
                this.#element.className = "col text-center cursor-pointer"
                break;
            case available:
                this.#element.className = "col text-center cursor-pointer text-white bg-success"
                break;
            case disabled:
                this.#element.className = "col text-center text-secondary"
                break;
            case outOfRange:
                this.#element.className = "col text-center text-secondary"
                this.#element.innerText = "-"
                break;
            default:
                console.log("Unknown status", s);
                return;
        }
        this.#status = s;
    }
    updateDayValue(v) {
        this.#element.innerText = v;
    }
    #handleClick(event) {
        if (this.#status === disabled || this.#status === outOfRange) {
            return;
        }

        if (this.#status === busy) {
            this.updateStatus(available)
        } else if (this.#status === available) {
            this.updateStatus(busy)
        } else {
            console.warn("Unexpected status value '%d'", this.#status);
        }
    }
}

// handlers
function handleUserNameClick(event) {
    currentUser = event.target.attributes["data-user-id"];
    fillCalendar(false);
}

function handleNewUserClick() {
    currentUser = newUsernameInput.value;
    fillCalendar(true);
}

function handlePrevMonthArrowClick() {
    if (compYearsMonths(fdMonth, minDate) <= 0) {
        return;
    }
    const newDate = new Date(fdMonth);
    newDate.setMonth(newDate.getMonth() - 1);
    updateMonth(fdMonth, newDate);
    if (compYearsMonths(fdMonth, minDate) <= 0) {
        prevMonthArr.style.display = "none";
    }
    nextMonthArr.style.display = "";
}

function handleNextMonthArrowClick() {
    if (compYearsMonths(fdMonth, maxDate) >= 0) {
        return;
    }
    const newDate = new Date(fdMonth);
    newDate.setMonth(newDate.getMonth() + 1);
    updateMonth(fdMonth, newDate);
    if (compYearsMonths(fdMonth, maxDate) >= 0) {
        nextMonthArr.style.display = "none";
    }

    prevMonthArr.style.display = "";
}

function fillAvailabilities(user, statuses, month) {
    let day = new Date(month);
    let index = 0;

    while (statuses[index] === outOfRange && index < 35) {
        index++; 
    }

    while (statuses[index] !== outOfRange && index < 35) {
        if (statuses[index] === available) {
            user.Availabilities.push({ date: day });
        }
        index++;
        day.setDate(day.getDate() + 1);
    }
}

function createUserAvailabilityDto() {
    const user = {
        Name: currentUser,
        PlanId: planId,
        Availabilities: []
    }

    const firstDay = new Date(minDate);
    firstDay.setDate(1);
    for (let month = firstDay; compYearsMonths(month, maxDate) < 0; month.setMonth(month.getMonth() + 1)) {
        if (compYearsMonths(month, fdMonth) == 0) {
            // get live data
            // allocating the new array here is kinda wasteful, but the size is really small so whatever.
            fillAvailabilities(user, dateElements.map(el => el.getStatus()), month);
        }

        // get data from cache
        const cached = getCachedStatuses(month.getTime());
        if (cached != null) {
            fillAvailabilities(user, cached, month)
        }
    }

    return user;
}

async function handleSaveButtonClick() {
    const data = createUserAvailabilityDto();

    try {
        const res = await fetch(userApiUrl, {
            method: "post",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!res.ok) {
            // TODO log & notify
        }

        // TODO redirect to plan page
    } catch (err) {
        // TODO notify failed save
        console.error(err);
    }
}

function fillCalendar(isNewUser) {
    // TODO
    // create calendar 
    // fill calendar with all the availabilities
    // fill calendar with free days selected by the user (if not new)
}

function getDaysInMonth(currentMonth) {
    const nextMonth = (currentMonth + 1) % 12; // Calculate the next month's index
    const currentDate = new Date(fdMonth);

    // Set the date to the last day of the current month
    currentDate.setMonth(nextMonth);
    currentDate.setDate(0);

    return currentDate.getDate();
}

function updateCache(key) {
    const statusesToCache = [35]
    for (let i = 0; i < 35; i++) {
        statusesToCache[i] = dateElements[i].getStatus();
    }
    statusCache.set(key, statusesToCache);
}

function getCachedStatuses(key) {
    if (statusCache.has(key)) {
        return statusCache.get(key);
    }

    return null;
}

function updateMonth(oldMonth, newMonth) {
    monthp.innerText = months[newMonth.getMonth() + 1]
    if (oldMonth !== newMonth) {
        updateCache(oldMonth.getTime());
    }
    const cachedStatuses = getCachedStatuses(newMonth.getTime());
    // update the date elements based on the first date of the month 
    const currentJsMonth = newMonth.getMonth();
    fdMonth.setMonth(currentJsMonth);
    let firstDayOfWeek = fdMonth.getDay();
    if (firstDayOfWeek === 0) {
        firstDayOfWeek = 6;
    } else {
        firstDayOfWeek--;
    }

    let dayIndex = 0;
    let el;
    while (dayIndex < firstDayOfWeek) {
        el = dateElements[dayIndex];
        el.updateStatus(outOfRange);
        dayIndex++;
    }

    let lastValidDate = dayIndex + getDaysInMonth(currentJsMonth);
    const currDate = new Date(fdMonth);
    while (dayIndex < lastValidDate) {
        el = dateElements[dayIndex];
        if (cachedStatuses !== null) {
            el.updateStatus(cachedStatuses[dayIndex]);
        } else {
            if (currDate >= minDate && currDate <= maxDate) {
                el.updateStatus(busy);
            } else {
                el.updateStatus(disabled);
            }
        }
        el.updateDayValue(currDate.getDate());
        currDate.setDate(currDate.getDate() + 1);
        dayIndex++;
    }

    while (dayIndex < 35) {
        el = dateElements[dayIndex];
        el.updateStatus(outOfRange);
        dayIndex++;
    }
}

export function getUIElements() {
    modal = document.getElementById("modal");
    userListItems = document.getElementsByClassName("list-group-item");
    newUsernameInput = document.getElementById("newUsernameInput");
    addButton = document.getElementById("addButton");
    saveButton = document.getElementById("saveBtn");
    monthp = document.getElementById("monthp");
    prevMonthArr = document.getElementById("prevMonthArr");
    nextMonthArr = document.getElementById("nextMonthArr");
    dateElements = [35];
    for (let i = 0; i < 35; i++) {
        dateElements[i] = new DateElement(document.getElementById("de_" + i));
    }
}

export function hookUpInteractionHandlers() {
    for (let i = 0; i < userListItems.length; i++) {
        userListItems[i].addEventListener("click", handleUserNameClick);
    }

    addButton.addEventListener("click", handleNewUserClick);
    prevMonthArr.addEventListener("click", handlePrevMonthArrowClick);
    nextMonthArr.addEventListener("click", handleNextMonthArrowClick);
    saveButton.addEventListener("click", handleSaveButtonClick);
}

function compYearsMonths(a, b) {
    const yearComp = a.getFullYear() - b.getFullYear();
    if (yearComp !== 0) {
        return yearComp;
    }

    return a.getMonth() - b.getMonth();
}

export function setApiUrls(user) {
    userApiUrl = user;
}

export function setServerdata(sMonths, sMinDate, sMaxDate, firstDayOfTheMonth, splanId) {
    months = sMonths;
    minDate = sMinDate;
    maxDate = sMaxDate;
    fdMonth = firstDayOfTheMonth;
    planId = splanId;
}

//  TODO check UTC date
export function run() {
    // set current month and update arrow display
    updateMonth(fdMonth, fdMonth);
    prevMonthArr.style.display = "none";
    if (compYearsMonths(fdMonth, maxDate) === 0) {
        nextMonthArr.style.display = "none";
    }

    // display modal
    const boostrapModal = new bootstrap.Modal(modal, {
        keyboard: false,
        focus: true
    });
    boostrapModal.show();
}

// TODO localize alert strings
// TODO user cookies?

// UI elements
let modal;
let userListItems;
let newUsernameInput;
let addButton;
let saveButton;
let monthp;
let prevMonthArr;
let nextMonthArr;
let alert;

// view data
let currentUserName; // will be used if the user is new
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

// urls 
let userApiUrl;
let fullPlanUrl;

// UI class
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

// cache helpers
function createKey(date) {
    // I want the key to be as small as possible, so a number will do
    // date within the same year/month have to resolve on the same key
    // let's do something really dumb, like year * 100 + month
    return date.getFullYear() * 100 + date.getMonth();
}

function prefillCache() {
    const date = new Date(minDate);
    date.setDate(1);

    while (compYearsMonths(date, maxDate) <= 0) {
        const cachedValues = [35];
        const currMonth = date.getMonth();
        let firstDayOfWeek = getFirstDayOfTheWeek(date);
        let dayIndex = 0;

        // run to the first actual day of the week in the calendar table
        while (dayIndex < firstDayOfWeek) {
            cachedValues[dayIndex++] = disabled;
        }

        // elaborate all the valid days
        let lastValidDate = dayIndex + getDaysInMonth(currMonth);
        const currDate = new Date(date);
        while (dayIndex < lastValidDate) {
            if (currDate >= minDate && currDate <= maxDate) {
                cachedValues[dayIndex++] = busy;
            } else {
                cachedValues[dayIndex++] = disabled;
            }
            currDate.setDate(currDate.getDate() + 1);
        }

        // finish up the rest
        while (dayIndex < 35) {
            cachedValues[dayIndex++] = disabled;
        }

        statusCache.set(createKey(date), cachedValues);
        date.setMonth(date.getMonth() + 1);
    }
}

function updateCache(key) {
    const statusesToCache = [35];
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

// date helpers
function padDay(date) {
    const temp = new Date(date);
    temp.setDate(1);
    return date.getDate() + getFirstDayOfTheWeek(temp) - 1;
}

function getDaysInMonth(currentMonth) {
    const nextMonth = (currentMonth + 1) % 12; // Calculate the next month's index
    const currentDate = new Date(fdMonth);

    // Set the date to the last day of the current month
    currentDate.setMonth(nextMonth);
    currentDate.setDate(0);

    return currentDate.getDate();
}

function getFirstDayOfTheWeek(date) {
    let firstDayOfWeek = date.getDay();
    if (firstDayOfWeek === 0) {
        return 6;
    } else {
        return --firstDayOfWeek;
    }
}

function compYearsMonths(a, b) {
    const yearComp = a.getFullYear() - b.getFullYear();
    if (yearComp !== 0) {
        return yearComp;
    }

    return a.getMonth() - b.getMonth();
}

// communication helpers
function fillAvailabilities(user, statuses, month) {
    let day = new Date(month);
    let index = 0;

    while (statuses[index] === outOfRange && index < 35) {
        index++;
    }

    while (statuses[index] !== outOfRange && index < 35) {
        if (statuses[index] === available) {
            user.Availabilities.push({ date: day.toJSON() });
        }
        index++;
        day.setDate(day.getDate() + 1);
    }
}

function createUserAvailabilityDto() {
    const user = {
        Name: currentUserName,
        PlanId: planId,
        Availabilities: []
    }

    const firstDay = new Date(minDate);
    firstDay.setDate(1);
    for (let month = firstDay; compYearsMonths(month, maxDate) <= 0; month.setMonth(month.getMonth() + 1)) {
        if (compYearsMonths(month, fdMonth) == 0) {
            // get live data
            // allocating the new array here is kinda wasteful, but the size is really small so whatever.
            fillAvailabilities(user, dateElements.map(el => el.getStatus()), month);
        }

        // get data from cache
        const cached = getCachedStatuses(createKey(month));
        if (cached != null) {
            fillAvailabilities(user, cached, month)
        }
    }

    return user;
}

// notification helpers
function notifyError(text) {
    alert.innerText = text;
    fadeIn(alert);
    setTimeout(() => {
        fadeOut(alert);
    }, 3000)
}

function fadeIn(element) {
    element.style.opacity = 0;
    element.style.display = 'block';

    var opacity = 0;
    var timer = setInterval(function () {
        if (opacity >= 1) {
            clearInterval(timer);
        }
        element.style.opacity = opacity;
        opacity += 0.1;
    }, 50);
}

function fadeOut(element) {
    var opacity = 1;
    var timer = setInterval(function () {
        if (opacity <= 0) {
            clearInterval(timer);
            element.style.display = 'none';
        }
        element.style.opacity = opacity;
        opacity -= 0.1;
    }, 50);
}

// handlers
async function handleUserNameClick(event) {
    const userId = event.target.attributes["data-user-id"].value;

    try {
        const res = await fetch(userApiUrl + "/" + userId);
        if (!res.ok) {
            console.error(res.statusText);
            notifyError("Failed to load user information");
            return;
        }

        const user = await res.json();
        const avail = user.availabilities;

        // fill the cache and then set all the availabilities
        prefillCache();

        // garbage implementation
        // this could be optimized by sorting and grouping all the dates
        // belonging to the same month together, but yet again, with the number
        // of elements I have to handle this shouldn't really matter
        for (let i = 0; i < avail.length; i++) {
            const date = new Date(avail[i].date);
            const day = padDay(date);
            date.setDate(1); // setting to first day of the month to match the key
            const cache = statusCache.get(createKey(date));
            cache[day] = available;
        }

        // run updateMonth to load the cache
        updateMonth(fdMonth, fdMonth);
    } catch (err) {
        console.error(err);
        notifyError("Failed to load user information");
    }

    modal.hide();
}

function handleNewUserClick() {
    currentUserName = newUsernameInput.value;
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

async function handleSaveButtonClick() {
    const data = createUserAvailabilityDto();

    try {
        // TODO remember to add unique constraint on user name 
        const res = await fetch(userApiUrl, {
            method: "post",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!res.ok) {
            console.error(res.statusText);
            notifyError("Failed to save your availabilities.");
            return;
        }

        // TODO planId 
        window.location.href = fullPlanUrl + "/" + planId; 
    } catch (err) {
        console.error(err);
        notifyError("Failed to save your availabilities.");
    }
}

function updateMonth(oldMonth, newMonth) {
    monthp.innerText = months[newMonth.getMonth() + 1]
    if (oldMonth !== newMonth) {
        updateCache(createKey(oldMonth));
    }
    const cachedStatuses = getCachedStatuses(createKey(newMonth));

    // update the date elements based on the first date of the month 
    const currMonth = newMonth.getMonth();
    fdMonth.setMonth(currMonth);
    let firstDayOfWeek = getFirstDayOfTheWeek(fdMonth);
    let dayIndex = 0;
    let el;

    // run to the first actual day of the week in the calendar table
    while (dayIndex < firstDayOfWeek) {
        el = dateElements[dayIndex];
        el.updateStatus(outOfRange);
        dayIndex++;
    }

    // elaborate all the valid days
    let lastValidDate = dayIndex + getDaysInMonth(currMonth);
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

    // finish up the rest
    while (dayIndex < 35) {
        el = dateElements[dayIndex];
        el.updateStatus(outOfRange);
        dayIndex++;
    }
}

// exports
export function getUIElements() {
    modal = new bootstrap.Modal(document.getElementById("modal"), {
        keyboard: false,
        focus: true
    });
    userListItems = document.getElementsByClassName("list-group-item");
    newUsernameInput = document.getElementById("newUsernameInput");
    addButton = document.getElementById("addButton");
    saveButton = document.getElementById("saveBtn");
    monthp = document.getElementById("monthp");
    prevMonthArr = document.getElementById("prevMonthArr");
    nextMonthArr = document.getElementById("nextMonthArr");
    alert = document.getElementById("alert");
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

export function setUrls(userApi, fullPlanRoute) {
    userApiUrl = userApi;
    fullPlanUrl = fullPlanRoute;
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
    modal.show();
}

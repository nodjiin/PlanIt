// UI elements
let modal;
let userListItems;
let newUsernameInput;
let addButton;
let monthp;
let prevMonthArr;
let nextMonthArr;

// view data
let currentUser;
let currentMonth;
let dateElements; 
let statusCache = new Map();
let fdMonth;

// server side data
let months;
let minMonth;
let maxMonth;

// single date status
// TODO status for availability of other users
const busy = 0;
const available = 1;
const disabled = 2;

class DateElement {
    #status = busy;
    #element;
    #date;
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
        if (this.#status === disabled) {
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
    if (currentMonth <= minMonth) {
        return;
    }
    updateMonth(currentMonth, --currentMonth);
    if (currentMonth === minMonth) {
        prevMonthArr.style.display = "none";
    }
    nextMonthArr.style.display = "";
}

function handleNextMonthArrowClick() {
    if (currentMonth >= maxMonth) {
        return;
    }
    updateMonth(currentMonth, ++currentMonth);
    if (currentMonth === maxMonth) {
        nextMonthArr.style.display = "none";
    }

    prevMonthArr.style.display = "";
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

function updateMonth(oldMonth, newMonth) {
    if (!Number.isInteger(newMonth) && newMonth > 0 && newMonth <= 12) {
        console.error("Invalid month number value: ", newMonth)
        newMonth = 0;
    }
    monthp.innerText = months[newMonth]
    if (oldMonth !== currentMonth) {
        // TODO save old status
        const statusesToCache = [35]
        for (let i = 0; i < 35; i++) {
            statusesToCache[i] = dateElements[i].getStatus();
        }
        statusCache.set(oldMonth, statusesToCache);
    }
    let cachedStatuses;
    if (statusCache.has(newMonth)) {
        cachedStatuses = statusCache.get(newMonth);
    }

    // update the date elements based on the first date of the month 
    const currentJsMonth = newMonth - 1;
    fdMonth.setMonth(currentJsMonth);
    let firstDayOfWeek = fdMonth.getDay();
    if (firstDayOfWeek === 0) {
        firstDayOfWeek = 6;
    } else {
        firstDayOfWeek--;
    }

    let dayIndex = 0;
    let el;
    // TODO extract repetition
    while (dayIndex < firstDayOfWeek) {
        el = dateElements[dayIndex];

        if (cachedStatuses !== undefined) {
            el.updateStatus(cachedStatuses[dayIndex]);
        } else {
            el.updateStatus(disabled);
        }
        el.updateDayValue("-");
        dayIndex++;
    }

    let lastValidDate = dayIndex + getDaysInMonth(currentJsMonth);
    let dayValue = 1;
    while (dayIndex < lastValidDate) {
        el = dateElements[dayIndex];
        if (cachedStatuses !== undefined) {
            el.updateStatus(cachedStatuses[dayIndex]);
        } else {
            el.updateStatus(busy);
        }
        el.updateDayValue(dayValue++);
        dayIndex++;
    }

    while (dayIndex < 35) {
        el = dateElements[dayIndex];
        if (cachedStatuses !== undefined) {
            el.updateStatus(cachedStatuses[dayIndex]);
        } else {
            el.updateStatus(disabled);
        }
        el.updateDayValue("-");
        dayIndex++;
    }
}

export function getUIElements() {
    modal = document.getElementById("modal");
    userListItems = document.getElementsByClassName("list-group-item");
    newUsernameInput = document.getElementById("newUsernameInput");
    addButton = document.getElementById("addButton");
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
}

export function run(sMonths, sMinMonth, sMaxMonth, firstDayOfTheMonth) {
    months = sMonths;
    minMonth = sMinMonth;
    currentMonth = minMonth;
    maxMonth = sMaxMonth;
    fdMonth = firstDayOfTheMonth;

    // set current month and update arrow display
    updateMonth(currentMonth, currentMonth);
    prevMonthArr.style.display = "none";
    if (currentMonth === maxMonth) {
        nextMonthArr.style.display = "none";
    }

    // display modal
    const boostrapModal = new bootstrap.Modal(modal, {
        keyboard: false,
        focus: true
    });
    boostrapModal.show();
}

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
let fdMonth;

// server side data
let months;
let minMonth;
let maxMonth;

// single date status
const busy = 0;
const available = 1;
const disabled = 2;

class DateElement {
    #status = busy;
    #element;
    constructor(de) {
        this.#element = de;
    }
    // TODO css classes
    updateStatus(s) {
        switch (s) {
            case busy:
                break;
            case available:
                break;
            case disabled:
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
    updateMonth(--currentMonth);
    if (currentMonth === minMonth) {
        prevMonthArr.style.display = "none";
    }
    nextMonthArr.style.display = "";
}

function handleNextMonthArrowClick() {
    if (currentMonth >= maxMonth) {
        return;
    }
    updateMonth(++currentMonth);
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

function updateMonth(number) {
    if (!Number.isInteger(number) && number > 0 && number <= 12) {
        console.error("Invalid month number value: ", number)
        number = 0;
    }
    monthp.innerText = months[number]

    // update the date elements based on the first date of the month 
    const currentJsMonth = number - 1;
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
        el = dateElements[dayIndex++];
        el.updateStatus(disabled);
        el.updateDayValue("-");
    }

    let lastValidDate = dayIndex + getDaysInMonth(currentJsMonth);
    let dayValue = 1;
    while (dayIndex < lastValidDate) {
        el = dateElements[dayIndex++];
        el.updateStatus(busy);
        el.updateDayValue(dayValue++);
    }

    while (dayIndex < 35) {
        el = dateElements[dayIndex++];
        el.updateStatus(disabled);
        el.updateDayValue("-");
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
    updateMonth(currentMonth);
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

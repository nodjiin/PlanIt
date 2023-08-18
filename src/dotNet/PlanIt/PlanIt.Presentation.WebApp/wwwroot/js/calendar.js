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

// server side data
let months;
let minMonth;
let maxMonth;

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
    // should be able to change from month to month
}

function updateMonth(number) {
    if (!Number.isInteger(number) && number > 0 && number <= 12) {
        console.error("Invalid month number value: ", number)
        number = 0;
    }
    monthp.innerText = months[number]
}

export function getUIElements() {
    modal = document.getElementById("modal");
    userListItems = document.getElementsByClassName("list-group-item");
    newUsernameInput = document.getElementById("newUsernameInput");
    addButton = document.getElementById("addButton");
    monthp = document.getElementById("monthp");
    prevMonthArr = document.getElementById("prevMonthArr");
    nextMonthArr = document.getElementById("nextMonthArr");
}

export function hookUpInteractionHandlers() {
    for (let i = 0; i < userListItems.length; i++) {
        userListItems[i].addEventListener("click", handleUserNameClick);
    }

    addButton.addEventListener("click", handleNewUserClick);
    prevMonthArr.addEventListener("click", handlePrevMonthArrowClick);
    nextMonthArr.addEventListener("click", handleNextMonthArrowClick);
}

export function run(sMonths, sMinMonth, sMaxMonth) {
    months = sMonths;
    minMonth = sMinMonth;
    currentMonth = minMonth;
    maxMonth = sMaxMonth;

    updateMonth(currentMonth);
    prevMonthArr.style.display = "none";
    if (currentMonth === maxMonth) {
        nextMonthArr.style.display = "none";
    }

    const boostrapModal = new bootstrap.Modal(modal, {
        keyboard: false,
        focus: true
    });

    boostrapModal.show();
}

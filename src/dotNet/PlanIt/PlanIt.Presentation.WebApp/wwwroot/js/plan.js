// small copypasta from calendar. the html component is the same but the interaction is way more limited. 
// the 2 scripts could be merged with a strategy pattern, but the overlap is so small that currently
// it's not worth the effort. 
import { createDateKey, compYearsMonths, getDaysInMonth, getFirstDayOfTheWeek } from "./dateHelper.js"

let monthp;
let prevMonthArr;
let nextMonthArr;
let backToCalendarBtn;

// view data
let planElements;
let fdMonth;

// server side data
let minDate;
let maxDate;
let planId;
let totUsers;

// localized strings
let months; // array containing the localized value for all the 12 months

// status of the single plan element 
const noMatch = 0;
const partialMatch = 1;
const fullMatch = 2;
const disabled = 3;
const outOfRange = 4;

// UI class
class PlanElement {
    #element;
    constructor(de) {
        this.#element = de;
        this.#element.addEventListener("hover", (w) => this.#handleHover(w));
    }
    updateStatus(s) {
        switch (s) {
            case noMatch:
                this.#element.className = "col text-center"
                break;
            case fullMatch:
                this.#element.className = "col text-center text-white bg-success"
                break;
            case disabled:
                this.#element.className = "col text-center text-secondary"
                break;
            case outOfRange:
                this.#element.className = "col text-center text-secondary"
                this.#element.innerText = "-"
                break;
            case partialMatch:
                this.#element.className = "col text-center text-white bg-warning"
                this.#element.innerText = "-"
                break;
            default:
                console.log("Unknown status", s);
                return;
        }
    }
    updateDayValue(v) {
        this.#element.innerText = v;
    }
    #handleHover() {

    }
}

// handlers
function handlePrevMonthArrowClick() {
    if (compYearsMonths(fdMonth, minDate) <= 0) {
        return;
    }
    fdMonth.setMonth(fdMonth.getMonth() - 1);
    updateMonth();
    if (compYearsMonths(fdMonth, minDate) <= 0) {
        prevMonthArr.style.display = "none";
    }
    nextMonthArr.style.display = "";
}

function handleNextMonthArrowClick() {
    if (compYearsMonths(fdMonth, maxDate) >= 0) {
        return;
    }

    fdMonth.setMonth(fdMonth.getMonth() + 1);
    updateMonth();
    if (compYearsMonths(fdMonth, maxDate) >= 0) {
        nextMonthArr.style.display = "none";
    }

    prevMonthArr.style.display = "";
}

function updateMonth() {
    monthp.innerText = months[fdMonth.getMonth() + 1]
    let firstDayOfWeek = getFirstDayOfTheWeek(fdMonth);
    let dayIndex = 0;
    let el;

    // run to the first actual day of the week in the calendar table
    while (dayIndex < firstDayOfWeek) {
        planElements[dayIndex++].updateStatus(outOfRange);
    }

    // elaborate all the valid days
    let lastValidDate = dayIndex + getDaysInMonth(fdMonth);
    const currDate = new Date(fdMonth);
    while (dayIndex < lastValidDate) {
        el = planElements[dayIndex];
        if (currDate >= minDate && currDate <= maxDate) {
            const users = getAvailabilitiesFromMap(currDate);
            if (users.length === 0) {
                el.updateStatus(noMatch);
            } else if (users.length === totUsers) {
                el.updateStatus(fullMatch);
            } else {
                el.updateStatus(partialMatch); 
            }
        } else {
            el.updateStatus(disabled);
        }
        el.updateDayValue(currDate.getDate());
        currDate.setDate(currDate.getDate() + 1);
        dayIndex++;
    }

    // finish up the rest
    while (dayIndex < 35) {
        planElements[dayIndex++].updateStatus(outOfRange);
    }
}

// availability map -> date / userNames[]
const avMap = new Map();

function addAvailabilityToMap(date, user) {
    const key = createDateKey(date);
    if (!avMap.has(key)) {
        const arr = [user];
        avMap.set(key, arr);
    } else {
        const arr = avMap.get(key);
        arr.push(user);
    }
}

function getAvailabilitiesFromMap(date) {
    const key = createDateKey(date);

    if (!avMap.has(key)) {
        return [];
    }

    return avMap.get(key);
}

function fillAvailabilitiesMap(users) {
    for (let i = 0; i < users.length; i++) {
        const user = users[i];
        if (user.availabilities === null || typeof user.availabilities === "undefined") {
            continue;
        }

        for (let j = 0; j < user.availabilities.length; j++) {
            const av = user.availabilities[j];
            addAvailabilityToMap(av.date, user.name);
        }
    }
}

function handleBackToCalendarBtnClick() {
        window.location.href = "/Plan/Calendar/" + planId;
}

// exports
export function getUIElements() {
    monthp = document.getElementById("monthp");
    prevMonthArr = document.getElementById("prevMonthArr");
    nextMonthArr = document.getElementById("nextMonthArr");
    backToCalendarBtn = document.getElementById("backToCalendarBtn"); 
    planElements = [35];
    for (let i = 0; i < 35; i++) {
        planElements[i] = new PlanElement(document.getElementById("de_" + i));
    }
}

export function hookUpInteractionHandlers() {
    prevMonthArr.addEventListener("click", handlePrevMonthArrowClick);
    nextMonthArr.addEventListener("click", handleNextMonthArrowClick);
    backToCalendarBtn.addEventListener("click", handleBackToCalendarBtnClick);
}

export function setLocalizedStrings(lMonths) {
    months = lMonths;
}

export function setServerdata(sMinDate, sMaxDate, sPlanId, users) {
    minDate = sMinDate;
    fdMonth = new Date(minDate);
    fdMonth.setDate(1);
    maxDate = sMaxDate;
    planId = sPlanId;
    totUsers = users.length;
    fillAvailabilitiesMap(users)
}

export function run() {
    // set current month and update arrow display
    updateMonth();
    prevMonthArr.style.display = "none";
    if (compYearsMonths(fdMonth, maxDate) === 0) {
        nextMonthArr.style.display = "none";
    }
}

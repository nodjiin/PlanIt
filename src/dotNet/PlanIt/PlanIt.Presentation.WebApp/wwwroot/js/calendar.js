// TODO new user intro to calendar
import { setCookie, getCookie, deleteCookie } from "./cookiesHelper.js"
import { notifyError } from "./alerts.js"
import { createMonthKey, compYearsMonths, getDaysInMonth, getFirstDayOfTheWeek, padDay } from "./dateHelper.js"

// UI elements
let modal;
let userListItems;
let newUsernameInput;
let addButton;
let saveButton;
let changeButton;
let monthp;
let prevMonthArr;
let nextMonthArr;

// view data
let currentUserName; // will be used if the user is new
let dateElements;
let statusCache = new Map();
let fdMonth;

// server side data
let minDate;
let maxDate;
let planId;

// localized strings
let months; // array containing the localized value for all the 12 months
let saveErr;
let loadErr;
let updateErr;
let invalidNameErr;

// single date status
const busy = 0;
const available = 1;
const disabled = 2;
const outOfRange = 3;

// Urls 
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
function prefillCache() {
    const date = new Date(minDate);
    date.setDate(1);

    while (compYearsMonths(date, maxDate) <= 0) {
        const cachedValues = [35];
        let firstDayOfWeek = getFirstDayOfTheWeek(date);
        let dayIndex = 0;

        // run to the first actual day of the week in the calendar table
        while (dayIndex < firstDayOfWeek) {
            cachedValues[dayIndex++] = outOfRange;
        }

        // elaborate all the valid days
        let lastValidDate = dayIndex + getDaysInMonth(date);
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
            cachedValues[dayIndex++] = outOfRange;
        }

        statusCache.set(createMonthKey(date), cachedValues);
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

function createUserAvailabilityDto(userId) {
    const user = {
        PlanId: planId,
        Availabilities: []
    }

    if (userId !== undefined) {
        user.Id = userId
    } else {
        user.Name = currentUserName
    }

    const firstDay = new Date(minDate);
    firstDay.setDate(1);
    for (let month = firstDay; compYearsMonths(month, maxDate) <= 0; month.setMonth(month.getMonth() + 1)) {
        if (compYearsMonths(month, fdMonth) == 0) {
            // get live data
            // allocating the new array here is kinda wasteful, but the size is really small so whatever.
            fillAvailabilities(user, dateElements.map(el => el.getStatus()), month);
            continue;
        }

        // get data from cache
        const cached = getCachedStatuses(createMonthKey(month));
        if (cached != null) {
            fillAvailabilities(user, cached, month)
        }
    }

    return user;
}

async function loadUser(userId) {
    try {
        const res = await fetch(userApiUrl + "/" + userId);
        if (!res.ok) {
            console.error(res.statusText);
            notifyError(loadErr);
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
            const cache = statusCache.get(createMonthKey(date));
            cache[day] = available;
        }

        // run updateMonth to load the cache
        updateMonth(fdMonth, fdMonth);
    } catch (err) {
        console.error(err);
        notifyError(loadErr);
    }
}

async function saveUser() {
    const data = createUserAvailabilityDto();
    const res = await fetch(userApiUrl, {
        method: "post",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    if (!res.ok) {
        if (res.status === 400) {
            notifyError(invalidNameErr + ` ${currentUserName}`);
        } else if (res.status === 500) {
            notifyError(saveErr);
        }

        console.error(res.statusText);
        return null;
    }

    return await res.json();
}

async function updateUser(userId) {
    const data = createUserAvailabilityDto(userId);
    const res = await fetch(userApiUrl, {
        method: "put",
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    if (!res.ok) {
        notifyError(updateErr);
        console.error(res.statusText);
        return false;
    }

    return true;
}

// handlers
async function handleUserNameClick(event) {
    const userId = event.target.attributes["data-user-id"].value;
    setCookie("userId", userId, 2);
    await loadUser(userId);
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
    try {
        let userId = getCookie("userId");
        if (userId != null) {
            if (!await updateUser(userId)) {
                return;
            }
        } else {
            userId = await saveUser();
            if (userId == null) {
                return;
            }

            setCookie("userId", userId, 2);
        }

        window.location.href = fullPlanUrl + "/" + planId;
    } catch (err) {
        console.error(err);
        notifyError(saveErr);
    }
}

function handleChangeButtonClick() {
    deleteCookie("userId");
    location.reload(); // it would also be possible to avoid the reload
}

function updateMonth(oldMonth, newMonth) {
    monthp.innerText = months[newMonth.getMonth() + 1]

    // update cache if switching to a new month
    if (oldMonth !== newMonth) {
        updateCache(createMonthKey(oldMonth));
    }

    // update the date elements based on the first date of the month 
    fdMonth.setMonth(newMonth.getMonth());

    // check if the month is present in the cache
    const cachedStatuses = getCachedStatuses(createMonthKey(newMonth));
    fillCalendar(newMonth, cachedStatuses);
}

function fillCalendar(newMonth, cachedStatuses) {
    let firstDayOfWeek = getFirstDayOfTheWeek(fdMonth);
    let dayIndex = 0;
    let el;

    // run to the first actual day of the week in the calendar table
    while (dayIndex < firstDayOfWeek) {
        dateElements[dayIndex++].updateStatus(outOfRange);
    }

    // elaborate all the valid days
    let lastValidDate = dayIndex + getDaysInMonth(newMonth);
    const currDate = new Date(fdMonth);
    while (dayIndex < lastValidDate) {
        el = dateElements[dayIndex];
        if (cachedStatuses !== null) { //restore the cached status if possible
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
        dateElements[dayIndex++].updateStatus(outOfRange);
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
    changeButton = document.getElementById("changeBtn");
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
    changeButton.addEventListener("click", handleChangeButtonClick)
}

export function setLocalizedStrings(lMonths, lSaveErr, lLoadErr, lUpdateErr, lInvalidNameErr) {
    months = lMonths;
    saveErr = lSaveErr;
    loadErr = lLoadErr;
    updateErr = lUpdateErr;
    invalidNameErr = lInvalidNameErr;
}

export function setUrls(userApi, fullPlanRoute) {
    userApiUrl = userApi;
    fullPlanUrl = fullPlanRoute;
}

export function setServerdata(sMinDate, sMaxDate, splanId) {
    minDate = sMinDate;
    fdMonth = new Date(minDate);
    fdMonth.setDate(1);
    maxDate = sMaxDate;
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
    const userId = getCookie("userId");
    if (userId === null) {
        modal.show();
    } else {
        loadUser(userId);
    }
}

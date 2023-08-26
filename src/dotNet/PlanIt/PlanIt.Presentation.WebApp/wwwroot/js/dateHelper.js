export function padDay(date) {
    const temp = new Date(date);
    temp.setDate(1);
    return date.getDate() + getFirstDayOfTheWeek(temp) - 1;
}

export function getDaysInMonth(date) {
    const currentDate = new Date(date);
    const nextMonth = (date.getMonth() + 1) % 12; // Calculate the next month's index

    // Set the date to the last day of the current month
    currentDate.setMonth(nextMonth);
    currentDate.setDate(0);

    return currentDate.getDate();
}

export function getFirstDayOfTheWeek(date) {
    let firstDayOfWeek = date.getDay();
    if (firstDayOfWeek === 0) {
        return 6;
    } else {
        return --firstDayOfWeek;
    }
}

export function compYearsMonths(a, b) {
    const yearComp = a.getFullYear() - b.getFullYear();
    if (yearComp !== 0) {
        return yearComp;
    }

    return a.getMonth() - b.getMonth();
}

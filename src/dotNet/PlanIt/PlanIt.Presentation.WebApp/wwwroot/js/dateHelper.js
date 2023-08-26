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

export function createMonthKey(date) {
    // I want the key to be as small as possible, so a number will do
    // date within the same year/month have to resolve on the same key
    // let's do something really dumb, like year * 100 + month
    return date.getFullYear() * 100 + date.getMonth();
}

// the key will be a number holding the day in the first 5 bits, the month in the next five bits and then the year 
export function createDateKey(date) {
    if (typeof date === "string") {
        // the date will be in the format yyyy-mm-ddThh:mm:ss
        if (date.length < 10) {
            console.error("date string invalid: '%s'", date)
            return null;
        }

        let temp = 0;
        let key = 0;
        temp = Number(date.substring(0, 4)); //year
        key |= temp << 10;
        temp = Number(date.substring(5, 7)) - 1; //month
        key |= temp << 5;
        temp = Number(date.substring(8, 10)); // day
        key |= temp;
        return key;
    } else if (date instanceof Date) {  
        let temp = 0;
        let key = 0;
        temp = date.getFullYear(); 
        key |= temp << 10;
        temp = date.getMonth(); 
        key |= temp << 5;
        temp = date.getDate(); 
        key |= temp;
        return key;
    }

    return null;
}

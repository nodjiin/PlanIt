export function setCookie(name, value, daysToExpire) {
    const expirationDate = new Date();
    expirationDate.setDate(expirationDate.getDate() + daysToExpire);
    const currentPath = window.location.pathname;
    const cookieValue = `${name}=${value}; expires=${expirationDate.toUTCString()}; path=${currentPath}`;

    document.cookie = cookieValue;
}

export function getCookie(name) {
    const cookies = document.cookie.split(';'); 
    
    for (const cookie of cookies) {
        const cookieParts = cookie.trim().split('=');
        const cookieName = cookieParts[0];
        const cookieValue = cookieParts[1];

        if (cookieName === name) {
            return decodeURIComponent(cookieValue); 
        }
    }

    return null; // Cookie with the specified name not found
}

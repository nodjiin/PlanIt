import { fadeIn, fadeOut } from "./animations.js"

const errAlert = document.getElementById("errAlert");

export function notifyError(text) {
    errAlert.innerText = text;
    fadeIn(errAlert);
    setTimeout(() => {
        fadeOut(errAlert);
    }, 3000)
}

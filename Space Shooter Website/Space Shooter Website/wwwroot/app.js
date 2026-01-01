function hidePopUp(text) {
    element = document.getElementById(text);
    if (element) { // Check if element exists to avoid potential errors
        const back_buttons_pics = document.querySelectorAll(".back");
        back_buttons_pics.forEach(pic => {
            pic.src = "/Assets/Main_Menu/BTNs_Active/Backward_BTN.png";
        });
        setTimeout(() => {
            element.style.display = 'none';
            back_buttons_pics.forEach(pic => {
                pic.src = "/Assets/Main_Menu/Setting/Backward_BTN.png";
            });
        }, 400);
    }
}

// Attach to window so the debugger always finds it
window.openSafeLink = function (url) {
    console.log("Debugger safe-opening: " + url);

    // We use a slight delay to let the Blazor event finish
    setTimeout(() => {
        const win = window.open(url, '_blank', 'noreferrer,noopener');
        if (win) {
            win.opener = null;
        }
    }, 100);
};

function print_error(msg) {
    var text = document.getElementById("error_text");
    if (text) {
        if (msg != "") {
            text.textContent = msg;
            text.style.display = 'Block';
            return true;
        }
        else {
            text.style.display = 'None';
        }
    }
    return false;
}
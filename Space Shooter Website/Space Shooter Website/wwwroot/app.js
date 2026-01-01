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

function openSafeLink(url) {
    console.log("Opening safe link to: " + url);
    const win = window.open(url, '_blank', 'noreferrer');
    if (win) { win.opener = null; }
}

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
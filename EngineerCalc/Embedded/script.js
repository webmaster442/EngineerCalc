function getTimeStamp() {
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    return `${hours}:${minutes}`;
}

function makeInputSafe(input) {
    return encodeURI(input).replace("%20", " ");
}

function render(input, result) {
    if (!input || input.trim() === "") return;
    const resultsDiv = document.getElementById('results');
    if (resultsDiv) {
        const html = `<div class="response"><p class="prompt">${getTimeStamp()}: '${makeInputSafe(input)}' &gt;</p>${result}</div>`;
        resultsDiv.insertAdjacentHTML("beforeend", html);
        const newItem = resultsDiv.lastElementChild;
        if (newItem) {
            newItem.scrollIntoView({ behavior: 'smooth', block: 'end' });
        }
    }
    else {
        console.warn('there is no results div');
    }
}

function handleClientCommand(input) {
    const resultsDiv = document.getElementById('results');
    if (!resultsDiv) {
        console.warn('there is no results div');
        return;
    }
    switch (input.toLowerCase()) {
        case '#clear':
            resultsDiv.innerHTML = "";
            return true;
        case '#reload':
            window.location.reload();
            return true;
        default:
            return false;
    }
}
async function execute(input) {
    try {
        let url = `/evaluate?e=${encodeURIComponent(input)}`;
        if (input.startsWith('#')) {
            if (handleClientCommand(input)) {
                return;
            }
            url = `/cmd?c=${encodeURIComponent(input)}`;
        }
        document.getElementById('loader').style.visibility = 'visible';
        const response = await fetch(url);
        let result = await response.text();
        if (!response.ok) {
            console.error(`Error (${response.status}): ${result}`);
            if (result === "") {
                result = `Error ${response.status}`;
            }
        }
        render(input, result);
    }
    catch (error) {
        render(input, error.message);
        console.error(error);
    }
    document.getElementById('loader').style.visibility = 'collapse';
}

async function intro() {
    try {
        document.getElementById('loader').style.visibility = 'visible';
        const response = await fetch("/intro");
        const result = await response.text();
        if (!response.ok) {
            throw new Error("Failed to get intro");
        }
        render("intro", result);
    }
    catch (error) {
        console.error(error);
    }
    document.getElementById('loader').style.visibility = 'collapse';
}

function type() {

}

document.addEventListener('DOMContentLoaded', () => {
    const inputElement = document.getElementById("input");
    if (!inputElement) {
        console.warn('no input element found');
        return;
    }
    intro();
    inputElement.focus();
    inputElement.addEventListener("keydown", (event) => {
        if (event.key == 'Enter') {
            execute(inputElement.value);
            inputElement.value = '';
            inputElement.focus();
        }
    });
});
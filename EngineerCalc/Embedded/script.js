function getTimeStamp() {
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    return `${hours}:${minutes}`;
}

function render(input, result) {
    if (!input || input.trim() === "") return;
    const resultsDiv = document.getElementById('results');
    if (resultsDiv) {
        const html = `<div class="response"><p class="prompt">${getTimeStamp()}: '${encodeURI(input)}' &gt;</p>${result}</div>`;
        resultsDiv.insertAdjacentHTML("beforeend", html);
    }
    else {
        console.warn('there is no results div');
    }
}

async function execute(input) {
    document.getElementById('loader').style.visibility = 'visible';
    try {
        const url = `/evaluate?e=${encodeURIComponent(input)}`
        const response = await fetch(url);
        const result = await response.text();
        if (!response.ok) {
            console.error(`Error (${response.status}): ${result}`);
        }
        render(input, result);
    }
    catch (error) {
        render(input, error.message);
        console.error(error);
    }
    document.getElementById('loader').style.visibility = 'collapse';
}

document.addEventListener('DOMContentLoaded', () => {
    const inputElement = document.getElementById("input");
    inputElement.focus();
    if (!inputElement) {
        console.warn('no input element found');
        return;
    }
    inputElement.addEventListener("keydown", (event) => {
        if (event.key == 'Enter') {
            execute(inputElement.value);
            inputElement.value = '';
            inputElement.focus();
        }
    });
});
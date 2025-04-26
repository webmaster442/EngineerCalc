let commands = [];

// ----------------------------------------------------------------------------

function getCommandsHtml() {
    let html = '<h1>Available commands</h1><ul>';
    for (i = 0; i < commands.length; i++) {
        html += `<li><a href="#" onclick="typeIntoInput(event)">${commands[i]}</a</li>`;
    }
    html += "</ul>";
    return html;
}

function getTimeStamp() {
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    return `${hours}:${minutes}`;
}

function makeInputSafe(input) {
    return encodeURI(input).replace("%20", " ");
}

function typeIntoInput(event) {
    event.preventDefault();
    const anchor = event.currentTarget;
    const text = anchor.textContent || anchor.innerText;
    const inputElement = document.getElementById("input");
    if (!inputElement) {
        console.warn('no input element found');
        return;
    }
    inputElement.value = text;
    inputElement.focus();
}

// ----------------------------------------------------------------------------

function render(input, result) {
    if (!input || input.trim() === "") return;

    const resultsDiv = document.getElementById('results');
    if (resultsDiv) {
        const html = `<div class="response"><p class="prompt">${getTimeStamp()}: '${makeInputSafe(input)}' &gt;</p>${result}</div>`;
        resultsDiv.insertAdjacentHTML("beforeend", html);
        const newItem = resultsDiv.lastElementChild;
        if (newItem) {
            newItem.scrollIntoView({ behavior: 'smooth', block: 'end' });
            MathJax.typesetPromise([newItem]).then(() => {
                console.log('Math typesetting complete!');
            });
        }
    }
    else {
        console.warn('there is no results div');
    }
}

function trySuggest(currentInput) {
    const suggestions = document.getElementById('suggestions');
    if (!suggestions) {
        return;
    }

    suggestions.innerHTML = "";

    if (!currentInput || currentInput === "") {
        return;
    }

    let html = "<span>Suggestions: </span>";
    const lowerInput = currentInput.toLowerCase();
    let wasAny = false;
    commands.forEach(command => {
        if (command.toLowerCase().startsWith(lowerInput)) {
            html += `<a href="#" onclick="typeIntoInput(event)">${command}</a>, `;
            wasAny = true;
        }
    });
    if (wasAny) {
        suggestions.insertAdjacentHTML("beforeend", html);
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
        case '#commands':
            render("#commands", getCommandsHtml());
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
        const introResponse = await fetch("/intro");
        if (!introResponse.ok) {
            throw new Error("Failed to get intro");
        }
        const introResult = await introResponse.text();

        const commandsResponse = await fetch("/commands");
        if (!commandsResponse.ok) {
            throw new Error("Failed to get intro");
        }
        const commandsResult = await commandsResponse.json();

        commands = commandsResult;
        commands.push("#clear", "#reload", "#commands");
        commands.sort();

        render("intro", introResult);
    }
    catch (error) {
        console.error(error);
    }
    document.getElementById('loader').style.visibility = 'collapse';
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
            trySuggest('');
            return
        }
        trySuggest(inputElement.value);
    });
});
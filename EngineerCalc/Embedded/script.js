function render(text) {
    if (!text || text.trim() === "") return;
    const resultsDiv = document.getElementById('results');
    if (resultsDiv) {
        resultsDiv.insertAdjacentHTML("beforeend", text);
    }
    else {
        console.warn('there is no results div');
    }
}

async function execute(input) {
    try {
        const url = `/evaluate?e=${encodeURIComponent(input)}`
        const response = await fetch(url);
        const result = await response.text();
        if (!response.ok) {
            console.error(`Error (${response.status}): ${result}`);
        }
        render(result);
    }
    catch (error) {
        render(error.message);
        console.error(error);
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const inputElement = document.getElementById("input");
    if (!inputElement) {
        console.warn('no input element found');
        return;
    }
    inputElement.addEventListener("keydown", (event) => {
        if (event.key == 'Enter') {
            execute(inputElement.value);
        }
    });
});
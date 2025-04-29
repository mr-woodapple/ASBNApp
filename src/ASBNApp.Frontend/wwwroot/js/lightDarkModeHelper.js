// Update the background color of the loading screen
function setThemeForLoader(loadingScreen, observer) {
    let darkLightThemeValue = (JSON.parse(localStorage.getItem('userPreferences') || '{}')).DarkLightTheme;

    let useLightTheme = darkLightThemeValue === 1 ||
        (darkLightThemeValue !== 2 && window.matchMedia('(prefers-color-scheme: light)').matches);

    if (useLightTheme) {
        // Set background-color for light theme
        loadingScreen.style.backgroundColor = '#ffffff';
    }

    observer.disconnect();
}

// Observes for DOM changes to detect the loading-screen element.
const loadingScreenObserver = new MutationObserver((mutationsList, observer) => {
    for (let mutation of mutationsList) {
        if (mutation.type === 'childList') {
            let loadingScreen = document.getElementsByClassName('loading-wrapper')[0];

            if (loadingScreen) {
                setThemeForLoader(loadingScreen, observer);
                break;
            }
        }
    }
});

// Start observing the document body for changes in the DOM.
loadingScreenObserver.observe(document.body, { childList: true, subtree: true });
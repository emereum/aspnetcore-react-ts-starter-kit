let baseUrlVal;
if (window.location.hostname.indexOf("localhost") > -1) {
    baseUrlVal = window.location.protocol + "//" + window.location.hostname + ":4000/api";
} else {
    baseUrlVal = window.location.protocol + "//" + window.location.hostname + "/api";
}

export const baseUrl = baseUrlVal;
export default class GuidService {
    static newGuid() {
        // See: https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
        return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(c: string) {
            // tslint:disable-next-line
            var r = Math.random()*16|0, v = c === "x" ? r : (r&0x3|0x8);
            return v.toString(16);
        });
    }
}
// https://gist.github.com/ca0v/73a31f57b397606c9813472f7493a940
export default function debounce<T extends Function>(cb: T, wait = 20) {
    let h:any = 0;
    let callable = (...args: any) => {
        clearTimeout(h);
        h = setTimeout(() => cb(...args), wait);
    };
    return <T>(<any>callable);
}
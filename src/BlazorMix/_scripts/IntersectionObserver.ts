

const obs = new WeakMap<Node, (el: IntersectionObserverEntry) => void>();

const viewPortObserver = new IntersectionObserver(
    (entries) => {
        for (const entry of entries) {
            if (entry.isIntersecting && obs.has(entry.target)) {
                const func = obs.get(entry.target);
                func!(entry)
            }
        }
    }
);

export const observeInViewportOnce = (dotNetObj: any, el: Element) => {
    obs.set(el, async (entry) => {
        obs.delete(entry.target);
        await dotNetObj.invokeMethodAsync("Invoke");
        viewPortObserver.unobserve(entry.target);
    })
    viewPortObserver.observe(el);
}
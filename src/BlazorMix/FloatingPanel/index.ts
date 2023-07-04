
import FloatingPanel from "./FloatingPanel"

const delObs = new WeakMap<Node, () => void>();
const observer = new MutationObserver((recordArr: MutationRecord[]) => {
    if (recordArr.length > 0 && recordArr[0].type === 'childList') {
        if (recordArr[0].removedNodes.length > 0) {
            const node = recordArr[0].removedNodes[0];
            const func = delObs.get(node);
            if (func) {
                func();
            }
            delObs.delete(node);
        }
    }
});
const insMap = new Map<any, FloatingPanel>();

export const init = (dotnetObj: any,
    root: HTMLElement,
    handleDraggingOfContent: boolean,
    anchors: number[],
    onHeightChangeDotnetObj: any
) => {
    const id = dotnetObj._id;
    if (insMap.has(id)) {
        return;
    }

    const mirror = new FloatingPanel(dotnetObj, root, handleDraggingOfContent, anchors, onHeightChangeDotnetObj);
    delObs.set(root, () => {
        mirror.dispose();
    })
    var observerOptions = {
        childList: true,
        subtree: true,
    }
    observer.observe(root, observerOptions);

    insMap.set(id, mirror)
    return mirror;
}



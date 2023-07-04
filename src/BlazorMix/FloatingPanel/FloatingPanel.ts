import { DragGesture } from "@use-gesture/vanilla";
import { nearest } from "../../util"
import { supportsPassive } from "../../supports-passive"


const prefix = "bm-floating-panel";
export default class FloatingPanel {
    private dotnetObj: any;
    private handleDraggingOfContent: boolean;
    private root: HTMLElement;
    private possibles: number[];
    private onHeightChangeDotnetObj: any;


    // @ts-ignore
    private dragGesture: DragGesture;
    private isPulling: boolean = false;
    private y: number;
    private bounds: {
        top: number,
        bottom: number
    };


    constructor(dotnetObj: any,
        root: HTMLElement,
        handleDraggingOfContent: boolean,
        anchors: number[],
        onHeightChangeDotnetObj: any
    ) {
        this.dotnetObj = dotnetObj;
        this.root = root;
        this.handleDraggingOfContent = handleDraggingOfContent;
        this.onHeightChangeDotnetObj = onHeightChangeDotnetObj;
        this.possibles = anchors.map(x => -x);
        this.bounds = {
            top: this.possibles[this.possibles.length - 1],
            bottom: this.possibles[0],
        }
        this.y = this.bounds.bottom;
        const maxHeight = anchors[anchors.length - 1] ?? window.innerHeight

        root.style.height = Math.round(maxHeight) + "px";
        root.style.transform = `translateY(calc(100 % + -${Math.round(this.bounds.bottom)}px))`;

        this.init();
    }

    init() {
        const header = this.root.querySelector(`.${prefix}-header`) as HTMLElement;
        const content = this.root.querySelector(`.${prefix}-content`) as HTMLElement;
        const { bounds } = this;
        this.dragGesture = new DragGesture(
            this.root,
            (state) => {
                //console.log("state", state.pressed, state.last, state);
                //let active = state.active;
                const [, offsetY] = state.offset;

                if (state.first) {
                    const target = state.event.target as Element
                    if (header === target || header?.contains(target)) {
                        this.isPulling = true;
                    } else {
                        if (!this.handleDraggingOfContent) return
                        const reachedTop = this.y <= bounds.top
                        if (!content) return
                        if (reachedTop) {
                            if (content.scrollTop <= 0 && state.direction[1] > 0) {
                                this.isPulling = true
                            }
                        } else {
                            this.isPulling = true
                        }
                    }
                }
                if (!this.isPulling) return
                const { event } = state
                if (event.cancelable) {
                    event.preventDefault()
                }
                event.stopPropagation()
                let nextY = offsetY
                if (state.last) {
                    this.isPulling = false
                    this.isPulling = false;
                    nextY = nearest(this.possibles, offsetY)
                }
                // console.log("this.updateY", this.possibles, offsetY, nextY);
                this.updateY({
                    y: nextY,
                })
            },
            {
                axis: 'y',
                bounds,
                rubberband: true,
                from: () => [0, this.y],
                pointer: { touch: true },
                target: this.root,
                eventOptions: supportsPassive
                    ? { passive: false }
                    : (false as unknown as AddEventListenerOptions),
            }
        );

    }

    setHeight(height: number, immediate?: boolean) {
        this.updateY({
            y: -height,
            // immediate: immediate
        })
    }

    async updateY(obj: any) {
        this.y = obj.y;
        this.root.style.transform = `translateY(calc(100% + ${this.y}px))`;
        await this.onHeightChangeDotnetObj.invokeMethodAsync("Invoke", -this.y);
    }


    dispose() {
        this.dragGesture?.destroy();
        this.onHeightChangeDotnetObj.dispose();
    }
}
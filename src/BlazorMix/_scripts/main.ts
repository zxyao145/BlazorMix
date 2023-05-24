
const Bm: any = {};

(window as any).Bm = Bm;

(window as any).process = {
    env: {
        NODE_ENV: "product"
    }
}

export default Bm
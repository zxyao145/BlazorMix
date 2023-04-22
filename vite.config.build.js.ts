import { defineConfig } from "vite";

const path = require("path");
const config = require("./package.json");

const banner = `/*!
* ${config.name} v${config.version} ${new Date()}
* (c) 2023 @BlazorMix
* Released under the MIT License.
*/`;

const { resolve } = path;

let fileStr = `@import "@/styles/variables.scss";`;
const projectID = process.env.VITE_APP_PROJECT_ID;
if (projectID) {
  fileStr = `@import '@/styles/variables-${projectID}.scss';`;
}



// https://vitejs.dev/config/
export default defineConfig(({ command, mode }) => {
    var isProd = mode === 'production';

    return {
        resolve: {
          alias: [{ find: "@", replacement: resolve(__dirname, "./src") }],
        },
        build: {
          minify: isProd,
          sourcemap: !isProd,
          emptyOutDir: false,
          outDir: "./src/BlazorMix/wwwroot/dist",
          rollupOptions: {
            output: {
              banner,
              // 入口文件 input 配置所指向的文件包名 默认值："[name].js"
              entryFileNames: (fileInfo) => {
                console.log("entryFileNames", fileInfo.facadeModuleId);
                return "[name].min.js";
              },
            },
          },
          lib: {
            entry: "./src/BlazorMix/wwwroot/src/ts/index.ts",
            name: "blazor-mix",
            fileName: "blazor-mix",
            formats: ["es"],
          },
        },
      }
}
);

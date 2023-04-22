import { defineConfig } from "vite";

const path = require("path");
const atImport = require("postcss-import");
const config = require("./package.json");

const banner = `/*!
* ${config.name} v${config.version} ${new Date()}
* (c) 2023 @BlazorMix
* Released under the MIT License.
*/`;

const { resolve } = path;
// https://vitejs.dev/config/
export default defineConfig(({ command, mode }) => {
  var isProd = mode === "production";
  return {
    resolve: {
      alias: [
        { find: "@", replacement: resolve(__dirname, "./src/BlazorMix/wwwroot/src/scss") },
      ],
    },
    esbuild: {
      drop: isProd ? ["console", "debugger"] : [],
    },
    css: {
      preprocessorOptions: {
        scss: {
          charset: false,
          additionalData: `@import "@/styles/variables.scss";`,
        },
        postcss: {
          plugins: [atImport({ path: path.join(__dirname, "src`") })],
        },
      },
    },
    plugins: [],
    build: {
      emptyOutDir: true,
      outDir: "./src/BlazorMix/wwwroot/dist",
      rollupOptions: {
        output: {
          banner,
          assetFileNames: (fileInfo) => {
            // console.log("fileInfo", fileInfo);
            // if (fileInfo.name == "style.css") {
            //   return "index.min.css";
            // }
            return `[name].[ext]`;
          },
          // 入口文件 input 配置所指向的文件包名 默认值："[name].js"
          entryFileNames: (fileInfo) => {
            console.log("entryFileNames", fileInfo.facadeModuleId);

            return "[name].min.js";
          },
        },
      },
      lib: {
        entry: {
          index: "./src/BlazorMix/wwwroot/src/ts/index.ts",
          default:
            "./src/BlazorMix/wwwroot/src/scss/styles/themes/default.scss",
        },
        name: "blazor-mix",
        fileName: "blazor-mix",
        formats: ["es"],
      },
    },
  };
});




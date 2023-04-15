import { defineConfig } from 'vite'

const path = require('path')
const atImport = require('postcss-import')
const config = require('./package.json')

const banner = `/*!
* ${config.name} v${config.version} ${new Date()}
* (c) 2023 @BlazorMix
* Released under the MIT License.
*/`

const { resolve } = path
// https://vitejs.dev/config/
export default defineConfig({
    resolve: {
        alias: [{ find: '@', replacement: resolve(__dirname, './src/BlazorMix') }],
    },
    css: {
        preprocessorOptions: {
            scss: {
                charset: false,
                additionalData: `@import "@/styles/variables.scss";`,
            },
            postcss: {
                plugins: [atImport({ path: path.join(__dirname, 'src`') })],
            },
        },
    },
    plugins: [],
    build: {
        emptyOutDir: false,
        outDir: './src/BlazorMix/wwwroot/dist',
        rollupOptions: {
            output: {
                banner,
            },
        },
        lib: {
            entry: './temp/styles/themes/default.scss',
            formats: ['es'],
            name: 'style',
            fileName: 'style',
        },
    },
})

const path = require('path')
const fs = require('fs-extra')
const glob = require('glob')
const componentsScss = glob.globSync('./src/BlazorMix/**/*.scss')
console.log("componentsScss", componentsScss);
let tasks = []

let fileStr = `@import '../theme-default.scss';\n@import '../variables.scss';\n`

componentsScss.map((cs) => {
  cs = cs.replaceAll('\\', '/')
  console.log(cs);
  if (cs.indexOf('src/BlazorMix/styles') > -1) {
    return
  }

  // cs: code source, example: src\BlazorMix\Button\index.scss
  const tempFilePath = `${cs.replace('src/BlazorMix/', 'components/')}`;
  const srcFile = path.resolve(__dirname, `../${cs}`);
  const targetFile = path.resolve(__dirname, `../temp`, tempFilePath);

  console.log(`${cs}, ${srcFile} --> ${targetFile}`);
  tasks.push(
    fs
      .copy(
        srcFile,
        targetFile
      )
      .catch((error) => {})
  )

  fileStr += `@import '../../${tempFilePath}';\n`
})

tasks.push(
  fs.copy(
    path.resolve(__dirname, '../src/BlazorMix/styles'),
    path.resolve(__dirname, '../temp/styles')
  )
)

Promise.all(tasks).then((res) => {
  fs.outputFile(
    path.resolve(__dirname, '../temp/styles/themes/default.scss'),
    fileStr,
    'utf8',
    (error) => {
      // logger.success(`文件写入成功`);
    }
  )
})

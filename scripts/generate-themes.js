const path = require('path')
const fs = require('fs-extra')
const glob = require('glob')
const componentsScss = glob.globSync('./src/BlazorMix/**/*.scss')
const outputDir = "./src/BlazorMix/wwwroot/src/scss"

let tasks = []


let fileStr = `@import '../theme-default.scss';\n@import '../variables.scss';\n\n`

componentsScss.map((cs) => {
  cs = cs.replaceAll('\\', '/')
  if (cs.indexOf('src/BlazorMix/_styles') > -1 || cs.indexOf("wwwroot") > -1) {
    return
  }

  // cs: code source, example: src\BlazorMix\Button\index.scss
  const tempFilePath = `${cs.replace('src/BlazorMix/', 'components/')}`;
  const srcFile = path.resolve(__dirname, `../${cs}`);
  const targetFile = path.resolve(__dirname, `../${outputDir}`, tempFilePath);

  // console.log(`${cs}, ${srcFile} --> ${targetFile}`);
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
    path.resolve(__dirname, '../src/BlazorMix/_styles'),
    path.resolve(__dirname, `../${outputDir}/styles`)
  )
)

Promise.all(tasks).then((res) => {
  fs.outputFile(
    path.resolve(__dirname, `../${outputDir}/styles/themes/default.scss`),
    fileStr,
    'utf8',
    (error) => {
      // logger.success(`文件写入成功`);
    }
  )
})

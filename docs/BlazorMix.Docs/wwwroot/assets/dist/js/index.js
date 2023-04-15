
const getId = (innerText) => {
    return innerText.toLocaleLowerCase().replaceAll(" ", "-") ?? "";
}

const tocActiveWatch = (hEles, tocEle) => {
    window.addEventListener('scroll', () => {
        const scrollTop = document.documentElement.scrollTop
        hEles.forEach((head, index) => {
            if ((head.offsetTop - scrollTop) < 200) {
                const path = location.pathname;
                const id = head.getAttribute("id");
                tocEle.querySelectorAll("a")
                    .forEach(a => {
                        if (a.classList?.contains("active")) {
                            a.classList.remove("active")
                        }
                    })
                console.log(`a[href="${path}#${id}"]`);
                tocEle.querySelector(`a[href="${path}#${id}"]`)
                    ?.classList.add("active")
            }
        })
    });
}


window.Mix = {
    Prism: {
        highlight: (code, language) => {
            //console.log(language, code);
            return Prism.highlight(code, Prism.languages[language], language);
        },
        highlightAll: () => {
            Prism.highlightAll();
        }
    },
    Docs: {
        renderToc: (articleElm, tocEle) => {
            console.log(articleElm, tocEle);
            const h1 = articleElm.querySelector("h1");
            h1?.setAttribute("id", getId(h1?.innerText));
            const hEles = articleElm.querySelectorAll("h2,h3");

            const path = location.pathname;
            let slugList = `<ul class="slug-list">`;
            for (const head of hEles) {
                if (head) {
                    const innerText = head.innerText;
                    let id = getId(innerText);
                    let depth = head.tagName[1];
                    head?.setAttribute("id", id);
                    slugList += `<li title="${innerText}" data-depth="${depth}">
                    <a href="${path}#${id}">${innerText}</a>
                    </li>`
                }
            }
            slugList += "</ul>";
            tocEle.innerHTML = slugList;
            tocActiveWatch(hEles, tocEle);
        },
        // https://www.meziantou.net/anchor-navigation-in-a-blazor-application.htm
        blazorScrollToId: (id) => {
            id = decodeURIComponent(id);
            const element = document.getElementById(id);
            if (element instanceof HTMLElement) {
                element.scrollIntoView({
                    behavior: "smooth",
                    block: "start",
                    inline: "nearest"
                });
            }
        }
    }
}


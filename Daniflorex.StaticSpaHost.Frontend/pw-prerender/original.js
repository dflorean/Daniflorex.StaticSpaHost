// @ts-check

import chalk from "chalk";
import fs from "fs-extra";
import minimist from "minimist";
import { dirname, extname, join } from "node:path";
import { fileURLToPath } from "node:url";
import { chromium } from "playwright";

const __dirname = dirname(fileURLToPath(import.meta.url));

const { version } = await fs.readJSON(join(__dirname, "package.json"), {
    encoding: "utf-8",
});

const argv = minimist(process.argv.slice(2), {
    string: ["url", "output", "load", "js", "selector", "sleep", "ua", "locale"],
    boolean: ["help", "show", "debug"],
    alias: { network: "load" },
});

const def = {
    loadTimeout: 30000,
    jsTimeout: 5000,
    sleep: 1000,
};

const options = {
    help: argv.h || argv.help || argv._[0] === "help",
    url: (argv.url || "").trim(),
    outputPath: (argv.output || "").trim(),
    loadTimeout: Number(argv.load) || def.loadTimeout,
    jsTimeout: Number(argv.js) || def.jsTimeout,
    domSelector: (argv.selector || "").trim(),
    sleep: Number(argv.sleep) || def.sleep,
    userAgent: (argv.ua || "").trim(),
    locale: (argv.locale || "").trim(),
    showBrowser: argv.show,
    debug: argv.debug,
};

if (options.debug) {
    console.log({ version, argv, parsedOptions: options });
}

main();

async function main() {
    if (options.help) {
        help();
    } else {
        let browser;

        try {
            if (options.url) {
                const url = options.url.startsWith("http")
                    ? options.url
                    : `https://${options.url}`;

                browser = await chromium.launch({ headless: !options.showBrowser });
                let context = await browser.newContext({
                    userAgent: options.userAgent,
                    locale: options.locale,
                });
                let page = await context.newPage();
                await page.goto(url, {
                    waitUntil: "load",
                    timeout: options.loadTimeout,
                });

                if (options.domSelector) {
                    await page.waitForSelector(options.domSelector, {
                        timeout: options.jsTimeout,
                    });
                } else {
                    await page.waitForTimeout(options.sleep);
                }

                const content = await page.content();

                if (options.outputPath) {
                    const outputFile = extname(options.outputPath)
                        ? options.outputPath
                        : `${options.outputPath}.html`;

                    await fs.writeFile(outputFile, content, { encoding: "utf-8" });
                } else {
                    console.log(content);
                }
            } else {
                console.log(`
${chalk.yellow(
                    "Missing required option --url. Run again with the --help option to see CLI help."
                )}`);
            }
        } catch (error) {
            throw error;
        } finally {
            browser && (await browser.close());
        }
    }
}
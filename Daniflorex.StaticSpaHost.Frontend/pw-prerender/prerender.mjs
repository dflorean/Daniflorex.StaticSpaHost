// @ts-check

import fs from "fs-extra";
import { chromium } from "playwright";

main();

async function main() {
    let browser;

    try {
                browser = await chromium.launch({ headless: false });
                let context = await browser.newContext({

                });
                let page = await context.newPage();
                await page.goto("https://localhost:7010/", {
                    waitUntil: "load",
                    timeout: 30000,
                });

                await page.waitForTimeout(5000);

            const content = await page.content();

            await fs.writeFile('.\\pw-prerender\\index.html', content, { encoding: "utf-8" });
            
    } catch (error) {
        throw error;
    } finally {
        browser && (await browser.close());
    }
}
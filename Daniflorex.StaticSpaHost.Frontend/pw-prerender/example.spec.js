// @ts-check

import fs from "fs-extra";
import { chromium } from "playwright";

const { test, expect } = require('@playwright/test');

test('Prerender Blazor SPA', async ({ page }) => {
    //await page.goto('https://localhost:7010/');
    let browser;
    browser = await chromium.launch({ headless: true });
    let context = await browser.newContext({

    });

    page = await context.newPage();

    await page.goto('https://localhost:7010/', {
        waitUntil: "load",
        timeout: 30000,
    });

    await page.waitForTimeout(1000);

    const content = await page.content();

    const outputFile = 'index.html';

    await fs.writeFile(outputFile, content, { encoding: "utf-8" });
});

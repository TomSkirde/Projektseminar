import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

/* -----------------------------------------------------------
   1) Ordner & Dateipfade für das Dev‑Zertifikat
----------------------------------------------------------- */

const baseFolder =
    env.APPDATA && env.APPDATA !== ''
        ? path.join(env.APPDATA, 'ASP.NET', 'https')
        : path.join(env.USERPROFILE ?? env.HOME ?? '', '.aspnet', 'https');

const certificateName = 'pass_workflow_engine.client';
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

/* -----------------------------------------------------------
   2) Zertifikat anlegen, falls nicht vorhanden
   (.NET 8: --format Pem erzeugt .pem + .key ohne Passwort)
----------------------------------------------------------- */

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

/* -----------------------------------------------------------
   3) Vite‑Konfiguration
----------------------------------------------------------- */
//Port aus dem Backend holen -> launchSettings.json
const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7291';


export default defineConfig({
    plugins: [react()],

    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/api': {
                target,
                secure: false,
                changeOrigin: true,
            }
        },
        port: parseInt(env.DEV_SERVER_PORT || '49762'),
        host: true, // Allow access from other devices in same network
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
});
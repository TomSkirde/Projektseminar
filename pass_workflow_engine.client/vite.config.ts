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
   (.NET 8: --format Pem erzeugt .pem + .key ohne Passwort)
----------------------------------------------------------- */

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    const result = child_process.spawnSync(
        'dotnet',
        [
            'dev-certs',
            'https',
            '--export-path',
            certFilePath,
            '--format',
            'Pem',
            '--no-password'
        ],
        { stdio: 'inherit' }
    );

    if (result.status !== 0) {
        throw new Error('Could not create development certificate.');
    }
}

/* -----------------------------------------------------------
   3) Vite‑Konfiguration
----------------------------------------------------------- */

export default defineConfig({
    plugins: [react()],

    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },

    server: {
        /* VS‑Standard‑Port — SpaProxy erkennt den Dev‑Server sofort */
        port: 5173,
        strictPort: true,

        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath)
        },

        proxy: {
            '/api': {
                target: 'https://localhost:7291', // ASP.NET‑Core‑Backend
                changeOrigin: true,
                secure: false, // self‑signed erlaubt
                ws: true       // WebSockets (SignalR) durchreichen
            }
        }
    }
});
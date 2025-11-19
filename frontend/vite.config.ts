import { defineConfig } from 'vite';
import angular from '@analogjs/vite-plugin-angular';
import { fileURLToPath } from 'node:url';
import { join } from 'node:path';

export default defineConfig({
  plugins: [
    angular({
      tsconfig: join(fileURLToPath(new URL('.', import.meta.url)), 'tsconfig.app.json')
    })
  ],
  resolve: {
    alias: {
      '@app': fileURLToPath(new URL('./src/app', import.meta.url)),
      '@env': fileURLToPath(new URL('./src/environments', import.meta.url))
    }
  },
  server: {
    port: 4200,
    open: true,
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        secure: false
      }
    }
  }
});

/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: ['./**/*.{razor,html}'],
  theme: {
    extend: {
      colors: {
        "jet-black": "#1E1E1E"
      }
    },
  },
  plugins: [],
}


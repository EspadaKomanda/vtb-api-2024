/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        customColor1: "#0E162D",
        customColor2: "#4A5068",
        customColor3: "#0085FF",
        customColor4: "#9A82FF",
      },
      backgroundColor: {
        'custom-blur': '#0E162D',
        'custom-bg-gray': '#4A5068'
      },
      backgroundImage: {
        'custom-gradient': 'linear-gradient(140deg , #9A82FF 35%, #0085FF 85%)',
      },
    },
  },
  plugins: [],
};

/** @type {import('tailwindcss').Config} */
module.exports = {
  mode: "jit",
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    extend: {
        gridTemplateColumns: {"12": "repeat(12, minmax(0, 1fr))","24": "repeat(24, minmax(0, 1fr))"},
        gridTemplateRows:{"12": "repeat(12, minmax(0, 1fr))","24": "repeat(24, minmax(0, 1fr))"},
        gridRow:{"span-12": "span 12 / span 12","span-13": "span 13 / span 13","span-14": "span 14 / span 14","span-15": "span 15 / span 15","span-16": "span 16 / span 16","span-17": "span 17 / span 17","span-18": "span 18 / span 18","span-19": "span 19 / span 19","span-20": "span 20 / span 20","span-21": "span 21 / span 21","span-22": "span 22 / span 22","span-23": "span 23 / span 23","span-24": "span 24 / span 24"},
        gridRowStart:{"12": "12","13": "13","14": "14","15": "15","16": "16","17": "17","18": "18","19": "19","20": "20","21": "21","22": "22","23": "23","24": "24","25": "25"},
        gridRowEnd:{"12": "12","13": "13","14": "14","15": "15","16": "16","17": "17","18": "18","19": "19","20": "20","21": "21","22": "22","23": "23","24": "24","25": "25"},
        gridColumn: {"span-12": "span 12 / span 12","span-13": "span 13 / span 13","span-14": "span 14 / span 14","span-15": "span 15 / span 15","span-16": "span 16 / span 16","span-17": "span 17 / span 17","span-18": "span 18 / span 18","span-19": "span 19 / span 19","span-20": "span 20 / span 20","span-21": "span 21 / span 21","span-22": "span 22 / span 22","span-23": "span 23 / span 23","span-24": "span 24 / span 24"},
        gridColumnStart: {  "12": "12",  "13": "13",  "14": "14",  "15": "15",  "16": "16",  "17": "17",  "18": "18",  "19": "19",  "20": "20",  "21": "21",  "22": "22",  "23": "23",  "24": "24",  "25": "25"},
        gridColumnEnd: {"12": "12","13": "13","14": "14","15": "15","16": "16","17": "17","18": "18","19": "19","20": "20","21": "21","22": "22","23": "23","24": "24","25": "25"}
      },
  },
  plugins: [],
}


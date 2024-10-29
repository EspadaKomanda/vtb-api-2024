
import { create } from 'zustand'

const useAccumulationStore = create((set) => ({
    tourName :  "Финал ВТБ 2024",
    setTourName: (name) => set({tourName: name}),
    sum: 140000,
    setSum: (sum) => set({sum: sum}),
    actualSum: 100000,
    setActualSum: (sum) => set({actualSum: sum})
}))

export default useAccumulationStore
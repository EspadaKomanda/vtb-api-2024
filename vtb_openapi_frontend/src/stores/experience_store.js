import { create } from 'zustand';

const experienceStore = create((set) => ({
    experiences: [],
    setExperiences: (experiences) => set({ experiences }),
    clearExperiences: () => set({ experiences: [] }),
}));

export default experienceStore;

import { create } from 'zustand'

const carouselStore = create((set) => ({
  slides: [],
  setSlides: (slides) => set({ slides }),
}));

export default carouselStore;

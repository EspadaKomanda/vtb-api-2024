import { create } from 'zustand';

const reviewsStore = create((set) => ({
    reviews: [],
    setReviews: (reviews) => set({ reviews }),
    clearReviews: () => set({ reviews: [] }),
}));

export default reviewsStore;

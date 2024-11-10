import {create} from 'zustand';

const auntificationStore = create((set) => ({
  isLoginOpen: false,
  isRegisterOpen: false,
  isAuthenticated: false,
  openLogin: () => set({ isLoginOpen: true }),
  closeLogin: () => set({ isLoginOpen: false }),
  openRegister: () => set({ isRegisterOpen: true }),
  closeRegister: () => set({ isRegisterOpen: false }),
  setIsAuthenticated: (value) => set({ isAuthenticated: value }),
}));

export default auntificationStore;

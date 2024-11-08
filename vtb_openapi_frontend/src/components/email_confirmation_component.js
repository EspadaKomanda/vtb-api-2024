import React, { useState } from 'react';
import auntificationStore from '@/stores/auntification_store.js';
import Image from 'next/image';
import * as img from '../assets/images.js';
import { motion } from 'framer-motion';
import Cookies from 'js-cookie';

const EmailConfirmation = () => {
  const [code, setCode] = useState('');
  const [error, setError] = useState('');
  const closeRegister = auntificationStore((state) => state.closeRegister);
  const [checkEmail, setCheckEmail] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!code) {
      setError('Пожалуйста, введите код подтверждения.');
      return;
    }
    setError('');
    setCheckEmail(true);

    try {
      const response = await fetch('', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ code }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Ошибка валидации, проверьте ваш код.');
      }

      const data = await response.json(); 
      closeRegister();
      Cookies.remove('firstName');
      Cookies.remove('lastName');
      Cookies.remove('middleName');
      Cookies.remove('birthDate');
      console.log('Почта подтверждена', data);
      console.log('Код подтверждения:', code);

    } catch (error) {
      setError(`Не удалось зарегистрироваться, попробуй другой логин (${error.message})`);
    } finally {
      setCheckEmail(false);
    }

    //TODO remove THIS, temporary for checking
    {closeRegister();
      Cookies.remove('firstName');
      Cookies.remove('lastName');
      Cookies.remove('middleName');
      Cookies.remove('birthDate');}
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="relative lg:w-1/4 sm:w-1/2 w-11/12 bg-custom-bg-gray rounded-lg p-6 shadow-lg">
        <motion.div
          initial={{ opacity: 0, y: -5 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.3, ease: 'easeInOut' }}
        >
          <h2 className="text-lg font-bold mb-4">Подтверждение почты</h2>
          <form onSubmit={handleSubmit}>
            <p className="mb-4">Мы отправили код подтверждения на вашу почту.</p>
            <input
              type="text"
              placeholder="Код подтверждения"
              value={code}
              onChange={(e) => setCode(e.target.value)}
              className="border p-2 mb-4 w-full bg-customColor1 rounded-md focus:outline-none border-none"
            />
            {error && <p className="text-red-500 mb-2">{error}</p>}
            <button type="submit" className="text-white p-2 rounded bg-custom-bg-blue px-5 font-semibold">
              {checkEmail ? 'Секунду' : 'Продолжить'}
            </button>
          </form>
          <button onClick={closeRegister} className="absolute top-6 right-6">
            <Image 
              src={img.exit} 
              alt="close" 
              width={30}
              height={30} 
              className='transition duration-300 hover:scale-110 active:scale-95' 
            />
          </button>
        </motion.div>
      </div>
    </div>
  );
};

export default EmailConfirmation;

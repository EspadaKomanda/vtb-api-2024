import React, { useState } from 'react';
import auntificationStore from '@/stores/auntification_store.js';
import Image from 'next/image';
import * as img from '../assets/images.js';

const EmailConfirmation = () => {
  const [code, setCode] = useState('');
  const [error, setError] = useState('');
  const closeRegister = auntificationStore((state) => state.closeRegister);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!code) {
      setError('Пожалуйста, введите код подтверждения.');
      return;
    }
    setError('');
    console.log('Код подтверждения:', code);
    closeRegister();
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="relative lg:w-1/4 sm:w-1/2 w-11/12 bg-custom-bg-gray rounded-lg p-6 shadow-lg">
        <h2 className="text-lg font-bold mb-4">Подтверждение почты</h2>
        <form onSubmit={handleSubmit}>
          <p className="mb-4">Мы отправили код подтверждения на вашу почту.</p>
          <input
            type="text"
            placeholder="Код подтверждения "
            value={code}
            onChange={(e) => setCode(e.target.value)}
            className="border p-2 mb-4 w-full bg-customColor1 rounded-md focus:outline-none border-none"
          />
          {error && <p className="text-red-500 mb-2">{error}</p>}
          <button type="submit" className="text-white p-2 rounded bg-custom-bg-blue px-5 font-semibold">Подтвердить</button>
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
      </div>
    </div>
  );
};

export default EmailConfirmation;

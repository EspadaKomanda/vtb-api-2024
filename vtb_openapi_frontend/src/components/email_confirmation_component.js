
import React, { useState } from 'react';
import auntificationStore from '@/stores/auntification_store.js';

const EmailConfirmation = () => {
  const [code, setCode] = useState('');
  const closeRegister = auntificationStore((state) => state.closeRegister);

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Код подтверждения:', code);
    closeRegister();
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white rounded-lg p-6 shadow-lg">
        <h2 className="text-lg font-bold mb-4">Подтверждение почты</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Введите код подтверждения"
            value={code}
            onChange={(e) => setCode(e.target.value)}
            className="border p-2 mb-2 w-full"
            />
            <button type="submit" className="bg-blue-500 text-white p-2 rounded">Подтвердить</button>
        </form>
        <button onClick={closeRegister} className="mt-4 text-red-500">Закрыть</button>
        </div>
    </div>
    );
};
    
    export default EmailConfirmation;
    
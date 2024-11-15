"use client";
import { useState } from 'react';

const LikeButton = ({ initialLiked = false, initialLikeCount = 0 }) => {
  const [liked, setLiked] = useState(initialLiked);
  const [likeCount, setLikeCount] = useState(initialLikeCount);

  const handleLikeToggle = async () => {

    //TODO here will be a request to the backend
    
    if (liked) {
      setLikeCount(likeCount - 1);
    } else {
      setLikeCount(likeCount + 1);
    }
    setLiked(!liked);
  };

  return (
    <button onClick={handleLikeToggle} className="flex items-center">
      <img
        src={liked ? '/images/like_active.svg' : '/images/like.svg'}
        alt="Like"
        className="h-8 mr-2 transition duration-300 hover:scale-110"
      />
      <span className="ml-1 text-lg">{likeCount}</span>
    </button>
  );
};

export default LikeButton;

const Pagination = ({ totalPages, currentPage, onPageChange }) => {
    const handlePageChange = (page) => {
        onPageChange(page);
    };

    const getPaginationButtons = () => {
        const buttons = [];
        const maxButtons = 5;
        const half = Math.floor(maxButtons / 2);

        let startPage = Math.max(1, currentPage - half);
        let endPage = Math.min(totalPages, currentPage + half);

        if (currentPage <= half) {
            endPage = Math.min(maxButtons, totalPages);
        }
        if (currentPage + half >= totalPages) {
            startPage = Math.max(1, totalPages - maxButtons + 1);
        }

        for (let i = startPage; i <= endPage; i++) {
            buttons.push(
                <button
                    key={i}
                    onClick={() => handlePageChange(i)}
                    className={`mx-1 px-3 py-1 rounded ${currentPage === i ? 'bg-blue-500 text-white' : 'bg-gray-300 text-black'}`}
                >
                    {i}
                </button>
            );
        }

        return buttons;
    };

    return (
        <div className="flex justify-center mt-4">
            {currentPage > 1 && (
                <button onClick={() => handlePageChange(currentPage - 1)} className="mx-1 px-3 py-1 rounded bg-gray-300 text-black">
                    Назад
                </button>
            )}
            {getPaginationButtons()}
            {currentPage < totalPages && (
                <button onClick={() => handlePageChange(currentPage + 1)} className="mx-1 px-3 py-1 rounded bg-gray-300 text-black">
                    Вперед
                </button>
            )}
        </div>
    );
};

export default Pagination;

const getRankingsByHostAndSearchTerm = async (host, searchTerm) => {
    await fetch(`https://localhost:44360/api/searchranking?host=${host}&searchTerm=${searchTerm}`);
};
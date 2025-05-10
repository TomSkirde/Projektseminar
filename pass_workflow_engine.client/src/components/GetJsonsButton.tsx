import { useState } from "react";



interface FileInfo {
    Name: string; // Groß‑/Kleinschreibung exakt wie im JSON!
    Path: string;
}

const GetJsonsButton: React.FC = () => {
    const [models, setModels] = useState<FileInfo[] | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [rawJson, setRawJson] = useState<string | null>(null); // State for raw JSON

    async function loadModels() {
        setLoading(true);
        setError(null);

        try {
            const res = await fetch('/api/Main/GetJsons', {
                headers: { Accept: 'application/json' }
            });

            if (!res.ok) throw new Error(`HTTP ${res.status}`);

            // 1.erstes JSON‑Parsing (liefert String)
            const payload = await res.json() as unknown;

            // 2.falls Server ein String‑Array schickt → nochmal parsen
            const data: FileInfo[] =
                typeof payload === 'string' ? JSON.parse(payload) : (payload as FileInfo[]);

            setModels(data);
            setRawJson(JSON.stringify(data, null, 2));
        } catch (err) {
            setError((err as Error).message);
        } finally {
            setLoading(false);
        }
    }

    return (
        <main style={{ padding: "2rem", fontFamily: "system-ui" }}>
            <button onClick={loadModels}>Modelle laden</button>

            {loading && <p>Lade</p>}
            {error && <p style={{ color: "red" }}>Fehler: {error}</p>}

            {models && (
                <ul>
                    {models.map(f => (
                        <li key={f.Path}>
                            {f.Name}
                            <br />
                            <small>{f.Path}</small>
                        </li>
                    ))}
                </ul>
            )}

            {rawJson && (
                <section>
                    <h3>Rohdaten:</h3>
                    <pre style={{ background: "#f4f4f4", padding: "1rem" }}>
                        {rawJson}
                    </pre>
                </section>
            )}
        </main>
    );
};

export default GetJsonsButton;
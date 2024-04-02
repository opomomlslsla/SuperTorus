import { useState } from 'react'
import './Styles/App.css'


//const [myvar, setmyvar] = useState();
function App() {
    const [RequestData, setParameters] = useState({
        A: 1,
        MaxRadius: 1,
        MinRadius: 1,
        Thickness: 1,
        Ncount: 1
    })

    const [result, SetResult] = useState({
        resultDouble: 0
    })

    const [message, Setmessage] = useState({
        error: ""
    })

    const FieldChangehandler = async () =>
    {
        //e.preventDefault();
        for (let key in RequestData) {
            //console.log(key);
            console.log(RequestData[key]);
            if (RequestData[key] == 0 || RequestData[key] == "") {
                return
            }
        }
        let response = await fetch('/Amin/Torus/TorusChek', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(RequestData)
        });
        let data = await response.json();
        console.log(data);
        Setmessage({
            error: data
        });

    }


    const handleSubmit = async (e) => {
        e.preventDefault();
        //response = await fetch("https://catfact.ninja/fact");
        //fetch('https://jsonplaceholder.typicode.com/todos/1')
        //    .then(response => response.json())
        //    .then(json => console.log(json))
        let response = await fetch('/Amin/Torus/TorusCalc', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(RequestData)
        });
        let data = await response.json();
        console.log(data);
        SetResult({
            resultDouble: data
        });

    }

    return (
        <div>

            <h1>
                {message.error}
                <br></br>
            </h1>
            <h1>
                {result.resultDouble}
                <br></br>
            </h1>

            <form onSubmit={handleSubmit}>
                <label>
                    Parametr A:
                    <input
                        required
                        type="number"
                        value={RequestData.A}
                        onChange={(e) => { setParameters({ ...RequestData, A: e.target.value }); FieldChangehandler(); }}
                    />
                </label>
                <br />
                <label>
                    MaxRadius:
                    <input
                        required
                        type="number"
                        value={RequestData.MaxRadius}
                        onChange={(e) => { setParameters({ ...RequestData, MaxRadius: e.target.value }); FieldChangehandler(); }}
                    />
                </label>
                <br />
                <label>
                    MinRadius:
                    <input
                        required
                        type="number"
                        value={RequestData.MinRadius}
                        onChange={(e) => { setParameters({ ...RequestData, MinRadius: e.target.value }); FieldChangehandler(); }}
                    />
                </label>
                <br />
                <label>
                    Thickness:
                    <input
                        required
                        type="number"
                        value={RequestData.Thickness}
                        onChange={(e) => { setParameters({ ...RequestData, Thickness: e.target.value }); FieldChangehandler(); }}
                    />
                </label>
                <br />
                <label>
                    Ncount:
                    <input 
                        required
                        type="number"
                        value={RequestData.Ncount}
                        onChange={(e) => { setParameters({ ...RequestData, Ncount: e.target.value }); FieldChangehandler(); }}
                    />
                </label>
                <br />
                <button type="submit">Submit</button>
            </form>
        </div>
    );
}






export default App

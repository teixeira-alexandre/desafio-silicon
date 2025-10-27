import React, { useEffect, useState } from 'react'
import { getSaldo, postDeposito, postSaque, postBloqueio, getExtrato } from '../api'
import ExtratoTabela from './ExtratoTabela'

export default function TelaConta({ idConta }){
  const [saldo, setSaldo] = useState(0)
  const [valor, setValor] = useState('')
  const [de, setDe] = useState('')
  const [ate, setAte] = useState('')
  const [extrato, setExtrato] = useState([])
  const [erro, setErro] = useState('')

  async function carregarSaldo(){
    setErro('')
    try{
      const r = await getSaldo(idConta)
      if(r.saldo !== undefined) setSaldo(r.saldo)
      else setErro(r.erro || 'Erro ao consultar saldo')
    }catch(e){ setErro('Falha na API') }
  }

  async function fazerDeposito(){
    setErro('')
    const v = parseFloat(valor)
    const r = await postDeposito(idConta, v)
    if(r.saldoAtual !== undefined){ setSaldo(r.saldoAtual); setValor('') }
    else setErro(r.erro || 'Erro no depósito')
  }

  async function fazerSaque(){
    setErro('')
    const v = parseFloat(valor)
    const r = await postSaque(idConta, v)
    if(r.saldoAtual !== undefined){ setSaldo(r.saldoAtual); setValor('') }
    else setErro(r.erro || 'Erro no saque')
  }

  async function bloquear(){
    setErro('')
    const r = await postBloqueio(idConta)
    if(!r.bloqueada){ setErro(r.erro || 'Erro ao bloquear') }
  }

  async function carregarExtrato(){
    setErro('')
    const r = await getExtrato(idConta, de || undefined, ate || undefined)
    if(Array.isArray(r)) setExtrato(r)
    else setErro(r.erro || 'Erro no extrato')
  }

  useEffect(()=>{ carregarSaldo(); carregarExtrato() }, [idConta])

  return (
    <div style={{marginTop:20, border:'1px solid #ccc', padding:16, borderRadius:8}}>
      <h3>Conta #{idConta}</h3>
      <div><b>Saldo:</b> R$ {saldo?.toFixed ? saldo.toFixed(2) : saldo}</div>
      <div style={{marginTop:10}}>
        <input placeholder='valor' value={valor} onChange={e=>setValor(e.target.value)} />
        <button onClick={fazerDeposito} style={{marginLeft:8}}>Depositar</button>
        <button onClick={fazerSaque} style={{marginLeft:8}}>Sacar</button>
        <button onClick={bloquear} style={{marginLeft:8, background:'#f55', color:'#fff'}}>Bloquear</button>
      </div>
      <div style={{marginTop:10}}>
        <b>Extrato por período:</b><br/>
        <input type='datetime-local' value={de} onChange={e=>setDe(e.target.value)} />
        <input type='datetime-local' value={ate} onChange={e=>setAte(e.target.value)} style={{marginLeft:8}}/>
        <button onClick={carregarExtrato} style={{marginLeft:8}}>Buscar</button>
      </div>
      {erro && <div style={{color:'red', marginTop:10}}>{erro}</div>}
      <ExtratoTabela itens={extrato} />
    </div>
  )
}

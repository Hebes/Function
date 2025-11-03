using System;
using System.Collections.Generic;
using UnityEngine;

public class FsmSystem
{
    private readonly Dictionary<string, IFsmStateNode> _nodes = new();
    private IFsmStateNode _curNode;//当前节点
    private IFsmStateNode _preNode;//上一个节点
    
    public void AddNode(Type type)
    {
        object stateNode = Activator.CreateInstance(type);
        if (string.IsNullOrEmpty(type.FullName)) throw new Exception("添加的节点为空");
        if (_nodes.ContainsKey(type.FullName)) throw new Exception($"状态节点已存在 : {type.FullName}");
        if (stateNode is not IFsmStateNode processStateNode) throw new Exception($"{type.FullName}没有继承接口");
        processStateNode.OnCreate(this);
        _nodes.Add(type.FullName, processStateNode);
    }
    
    public void ChangeState(string nodeName, object obj = null)
    {
        var node = TryGetNode(nodeName);
        if (node == null)
            throw new Exception($"没有找到状态节点 : {nodeName}");
        Debug.Log($"{_curNode.GetType().FullName} --> {node.GetType().FullName}");
        _preNode = _curNode;
        _curNode.OnExit();
        _curNode = node;
        _curNode.OnEnter(obj);
    }
    
    public void Run(string entryNode, object obj = null)
    {
        _curNode = TryGetNode(entryNode);
        Debug.Log($"Run -->{entryNode}");
        _preNode = _curNode;
        if (_curNode == null)
            throw new Exception($"未找到进入的节点: {entryNode}");
        _curNode.OnEnter(obj);
    }
    private IFsmStateNode TryGetNode(string nodeName)
    {
        _nodes.TryGetValue(nodeName, out IFsmStateNode result);
        return result;
    }
    
}

public interface IFsmStateNode
{
    /// <summary>
    /// 创建出来的时候会执行
    /// </summary>
    /// <param name="obj"></param>
    void OnCreate(object obj);

    /// <summary>
    /// 状态切换的时候会执行
    /// </summary>
    void OnEnter(object obj);

    /// <summary>
    /// 轮询
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// 退出
    /// </summary>
    void OnExit();
}